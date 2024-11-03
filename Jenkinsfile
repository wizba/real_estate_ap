pipeline {
    agent any
    
    options {
        timeout(time: 1, unit: 'HOURS')
    }
    
    environment {
        // AWS Credentials - Single credential binding
        AWS_ACCESS = credentials('AWS_WILLIAM_ADMIN')
        
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
                echo "Current directory:"
                cd
                echo "Directory contents:"
                dir
                
                if not exist publish (
                    echo "Error: Publish directory not found"
                    exit /b 1
                )
                
                echo "Publish directory contents:"
                dir publish
                
                if exist publish.zip del publish.zip
                powershell -Command "Compress-Archive -Force -Path 'publish\\*' -DestinationPath 'publish.zip'"
                
                echo "Checking if zip was created:"
                dir publish.zip
            '''
        }
    }

    stage('Upload to S3') {
        steps {
            bat '''
                echo "Verifying zip file exists:"
                dir publish.zip
                
                if not exist publish.zip (
                    echo "Error: publish.zip not found"
                    exit /b 1
                )
                
                aws s3 cp publish.zip s3://%BUCKET_NAME%/%APP_PACKAGE% ^
                --region %AWS_REGION%
            '''
        }
    }

    stage('Deploy to EC2') {
        steps {
            bat '''
                aws ssm send-command ^
                --instance-ids %INSTANCE_ID% ^
                --document-name "AWS-RunShellScript" ^
                --parameters commands=["mkdir -p /tmp/deploy","aws s3 cp s3://%BUCKET_NAME%/%APP_PACKAGE% /tmp/deploy/","sudo systemctl stop dotnet-app","sudo rm -rf %APP_PATH%/*","cd /tmp/deploy && unzip -o %APP_PACKAGE%","sudo cp -r /tmp/deploy/publish/* %APP_PATH%/","sudo chown -R ec2-user:ec2-user %APP_PATH%/","sudo systemctl start dotnet-app","rm -rf /tmp/deploy"]
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
                        --instance-ids %INSTANCE_ID% ^
                        --document-name "AWS-RunShellScript" ^
                        --parameters commands=["systemctl is-active dotnet-app"]
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
            cleanWs()
        }
        success {
            echo 'Pipeline executed successfully!'
        }
        failure {
            echo 'Pipeline execution failed!'
        }
    }
}