pipeline {
    agent any
    
    options {
        timeout(time: 1, unit: 'HOURS')
    }
    
    environment {
        // AWS Credentials - Using Jenkins credentials
        AWS_ACCESS_KEY_ID = credentials('AWS_WILLIAM_ADMIN').AccessKeyId
        AWS_SECRET_ACCESS_KEY = credentials('AWS_WILLIAM_ADMIN').SecretAccessKey
        
        // Environment Variables
        AWS_REGION = "${env.AWS_REGION_EAST}"
        INSTANCE_ID = "${env.EC2_INSTANCE_ID}"
        BUCKET_NAME = "${env.REAL_ESTATE_S3_BUCKET}"
        APP_PACKAGE = "${env.S3_KEY}"
        APP_PATH = "/var/www/dotnet"
        
        // Ensure AWS CLI uses these credentials
        AWS_DEFAULT_REGION = "${env.AWS_REGION_EAST}"
    }

    stages {
        stage('Validate Environment') {
            steps {
                script {
                    if (!AWS_REGION?.trim()) {
                        error "AWS_REGION is not set"
                    }
                    if (!INSTANCE_ID?.trim()) {
                        error "INSTANCE_ID is not set"
                    }
                    if (!BUCKET_NAME?.trim()) {
                        error "BUCKET_NAME is not set"
                    }
                    if (!APP_PACKAGE?.trim()) {
                        error "APP_PACKAGE is not set"
                    }
                    
                    echo """
                        Using Configuration:
                        AWS Region: ${AWS_REGION}
                        Instance ID: ${INSTANCE_ID}
                        Bucket: ${BUCKET_NAME}
                        Package: ${APP_PACKAGE}
                        Deploy Path: ${APP_PATH}
                    """
                }
            }
        }

        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        
        stage('Restore') {
            steps {
                bat 'dotnet restore'
            }
        }
        
        stage('Build') {
            steps {
                bat 'dotnet build --configuration Release'
            }
        }
        
        stage('Test') {
            steps {
                bat 'dotnet test'
            }
        }
        
        stage('Publish') {
            steps {
                bat 'dotnet publish -c Release -o ./publish'
            }
        }
        
        stage('Package') {
            steps {
                bat '''
                    if not exist publish (
                        echo Publish directory not found
                        exit /b 1
                    )
                    if exist publish.zip del publish.zip
                    powershell -Command "Compress-Archive -Force -Path 'publish\\*' -DestinationPath 'publish.zip'"
                '''
            }
        }
        
        stage('Upload to S3') {
            steps {
                bat "aws s3 cp publish.zip s3://%BUCKET_NAME%/%APP_PACKAGE%"
            }
        }
        
        stage('Deploy to EC2') {
            steps {
                bat '''
                    aws ssm send-command ^
                    --region %AWS_REGION% ^
                    --instance-ids %INSTANCE_ID% ^
                    --document-name "AWS-RunShellScript" ^
                    --comment "Deploying application" ^
                    --parameters commands=["mkdir -p /tmp/deploy", ^
                                        "aws s3 cp s3://%BUCKET_NAME%/%APP_PACKAGE% /tmp/deploy/", ^
                                        "sudo systemctl stop dotnet-app", ^
                                        "sudo rm -rf %APP_PATH%/*", ^
                                        "cd /tmp/deploy && unzip -o %APP_PACKAGE%", ^
                                        "sudo cp -r /tmp/deploy/publish/* %APP_PATH%/", ^
                                        "sudo chown -R ec2-user:ec2-user %APP_PATH%", ^
                                        "sudo systemctl start dotnet-app", ^
                                        "rm -rf /tmp/deploy"] ^
                    --output text
                '''
            }
        }
        
        stage('Health Check') {
            steps {
                sleep(30)
                script {
                    def result = bat(
                        script: '''
                            aws ssm send-command ^
                            --region %AWS_REGION% ^
                            --instance-ids %INSTANCE_ID% ^
                            --document-name "AWS-RunShellScript" ^
                            --comment "Health check" ^
                            --parameters commands=["systemctl is-active dotnet-app"] ^
                            --output text
                        ''',
                        returnStatus: true
                    )
                    
                    if (result != 0) {
                        error "Health check failed: Service is not active"
                    }
                }
            }
        }
    }

    post {
        always {
            echo """
                Pipeline completed with following configurations:
                AWS Region: ${AWS_REGION}
                Instance ID: ${INSTANCE_ID}
                Bucket: ${BUCKET_NAME}
                Package: ${APP_PACKAGE}
                Deploy Path: ${APP_PATH}
            """
        }
        success {
            echo 'Pipeline executed successfully!'
        }
        failure {
            echo 'Pipeline execution failed!'
        }
    }
}