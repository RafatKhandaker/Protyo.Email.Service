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

