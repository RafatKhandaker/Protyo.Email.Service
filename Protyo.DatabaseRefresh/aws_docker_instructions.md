
// In order to do this you need to have AWS Console cli and configure access keys with AWS IAM Provider
aws configure

//take docker file and copy it into the solution "root level"" Dockerfile
docker build -t protyo.databaserefresh .

// find docker Image
docker images

// login to AWS Service
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 887065820861.dkr.ecr.us-east-1.amazonaws.com

// associate docker image tag to aws ecr repository
docker tag protyo.databaserefresh:latest 887065820861.dkr.ecr.us-east-1.amazonaws.com/grant_database_refresh:Latest-1.2

// push image into ECR Repository
docker push 887065820861.dkr.ecr.us-east-1.amazonaws.com/grant_database_refresh:Latest-1.2


