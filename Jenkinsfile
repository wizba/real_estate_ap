pipeline {
    agent any
    
    environment {
        // AWS Credentials
        AWS_CREDENTIALS = credentials('AWS_WILLIAM_ADMIN')
        
        // Environment Variables
        AWS_REGION = "${env.AWS_REGION_EAST}"
        INSTANCE_ID = "${env.EC2_INSTANCE_ID}"
        BUCKET_NAME = "${env.REAL_ESTATE_S3_BUCKET}"
        APP_PACKAGE = "${env.S3_KEY}"
        APP_PATH = "/var/www/dotnet"
    }

    stages {
        stage('Validate Environment') {
            steps {
                script {
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
                script {
                    bat '''
                        powershell -Command "if (Test-Path .\\publish) {
                            Compress-Archive -Path .\\publish\\* -DestinationPath .\\publish.zip -Force
                            Write-Host 'Package created successfully'
                        } else {
                            Write-Error 'Publish directory not found'
                            exit 1
                        }"
                    '''
                }
            }
        }   
        
        stage('Upload to S3') {
            steps {
                withAWS(credentials: 'AWS_WILLIAM_ADMIN', region: AWS_REGION) {
                    bat "aws s3 cp publish.zip s3://${BUCKET_NAME}/${APP_PACKAGE}"
                }
            }
        }
        
        stage('Deploy to EC2') {
            steps {
                withAWS(credentials: 'AWS_WILLIAM_ADMIN', region: AWS_REGION) {
                    bat """
                        aws ssm send-command --instance-ids ${INSTANCE_ID} --document-name "AWS-RunShellScript" --parameters commands=[^
                            "mkdir -p /tmp/deploy",^
                            "aws s3 cp s3://${BUCKET_NAME}/${APP_PACKAGE} /tmp/deploy/",^
                            "sudo systemctl stop dotnet-app",^
                            "sudo rm -rf ${APP_PATH}/*",^
                            "cd /tmp/deploy && unzip -o ${APP_PACKAGE}",^
                            "sudo cp -r /tmp/deploy/publish/* ${APP_PATH}/",^
                            "sudo chown -R ec2-user:ec2-user ${APP_PATH}",^
                            "sudo systemctl start dotnet-app",^
                            "rm -rf /tmp/deploy"^
                        ]
                    """
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