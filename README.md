# Protyo Email Service

This system is designed within Protyo Organization in order to create a subscription system for Government Grant Programs.
The Goal and objective of this application is to act as an intermediary middleware worker service system which collects 
information retrieved from a google-form data responses and then creates a subscription email listing.

The email listing will integrate with an ML Operations infrastructure design solution in AWS, Utilizing Hermes 2 Pro Function-Calling Action model;
Which will create a Criteria Match case system with LLMs in order to match subscribed Users to a Tiered Level Government Grant Program.

This Open Source application will not run without a Security folder: Protyo.EmailService/Security  
with the added files: service-credentials.json file created from a Google API Service Account 


![image](https://github.com/RafatKhandaker/Protyo.Email.Service/assets/19369242/f6e10c51-1877-4623-8e87-bf4953875c2b)

![image](https://github.com/RafatKhandaker/Protyo.Email.Service/assets/19369242/1ec5ada9-c5a7-49b5-a29f-92158c3c109e)

![image](https://github.com/RafatKhandaker/Protyo.Email.Service/assets/19369242/70f63c5b-5d0e-481e-b14e-b9de55cb5417)



# Architecture Design 

Protyo Email Subscription Service, Utilizes Web API Service to Communicate with Stripe API Endpoint. Subscription Service Contact ML Operations LLM Model. 
Users will Interact with Google forms, which will submit response data of consumer for subscription service.  
Protyo.DatabaseRefresh will refresh a database job, updating database information from https://Grants.gov to archive grant loan information.
This information will be fed into the Sage Maker Pipelines for the ML Operations Infrastructure to train ML engineering data for a Grant Matching system using 
Hermes 2 Function Calling with LLM integration with GPT4. 

Protyo.EmailSubscriptionService will consume sage maker endpoint model data for a list of matching grants for each consumer, 
combined with the contact information from Google Forms, will generate an HTML formatted Email system to help users subscribe into a matching Grant system.


![image](https://github.com/RafatKhandaker/Protyo.Email.Service/assets/19369242/f8b2869e-c6ec-4dc9-be1e-d166158dfaca)



# Protyo Web Service

Protyo Web Service exposes Grant Information for the ML Engineering team to interact with. The Webservice is integrated with a Caching mechanism for both Grants
and Google Sheets API to save IOP Costs to both AWS & Google Cloud Services. Filters allow ML team to filter quality grants to define more specific cost ranges
for Grants. Model Schema is available to view on the Web Service, provided by Swagger UI.


![image](https://github.com/RafatKhandaker/Protyo.Email.Service/assets/19369242/4981ac83-6039-4b3c-9f24-9cb9520d4c4e)

![image](https://github.com/RafatKhandaker/Protyo.Email.Service/assets/19369242/104a494d-307e-4ea3-82c6-60fd3aa24c5d)



# Worker Service Jobs

Protyo Worker Service Jobs are designed to run on Windows Server as a Service but can also be scaled into dockerized containers. The Logging information system 
logs error and information of the worker service jobs and errors into Windows Event Viewer. Easy Management and maintenance of service in any infrastructure environment.
The Worker Services application runs on windows server as a local background service. Through the Services.msc applet. The Database refresh job is configured to run
once every 24 hours to check the Grants API database for any updated grant information that is not stored within the inhouse dynamo DB in AWS. The Algorithm in
Database refresh job will cross validate with existing grant id keys within the database to prevent duplicate records from syncing in house. 


![image](https://github.com/RafatKhandaker/Protyo.Email.Service/assets/19369242/527e9474-ec1f-4ff3-b273-579a852e4286)

![image](https://github.com/RafatKhandaker/Protyo.Email.Service/assets/19369242/a53d031e-830d-4cf1-90d3-d0824d7c4ebe)

![image](https://github.com/RafatKhandaker/Protyo.Email.Service/assets/19369242/5765a685-5b7b-4600-92f1-dc047a9ea0b6)



# Email Subscription Service
The Email Subscription service is a worker service job and running the process will send an html formatted email to the list of subscribers collected from the google forms


![image](https://github.com/RafatKhandaker/Protyo.Email.Service/assets/19369242/bbd908ca-9371-4375-abee-63fcfb0f6891)



