# SpeechQnAAssistant
Azure Speech + LUIS + QnA maker 
## Overview
This demo is for Speech chatbot with Speech + LUIS + QnA maker (without Bot Framework) to help you build a voice chat assistant much more faster and easier. 
## Prerequisites
Azure speech service:
https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/overview#create-the-azure-resource

QnA Maker:
https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/quickstarts/create-publish-knowledge-base?tabs=v1

LUIS Service:
https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-get-started-create-app#sign-in-to-luis-portal

## Getting the samples
Download .zip file, extract all.

## Configuration
#### Speech service configuration (./SS.Common.AI/SpeechHelper.cs): 

Set your "subscriptionKey" & "region".

All speech service related operations are defined here (STT/TTS to mp3/wav/speaker).


#### QnA Maker (./SS.Common.AI/QnAMakerHelper.cs):

Set your "authoringKey" & "resourceName" & "kbId".

All QnA service related operations are defined here.


#### LUIS (./SS.Common.AI/SpeechHelper.cs):

Set your "authoringKey" & "predictionKey" & "authoringResourceName" & "predictionResourceName" & "appName" & "appId".

All LUIS service related operations are defined here.

## Run the project
The result should be like this:

![image](https://github.com/twimkle0117/SpeechQnAAssistant/blob/main/Result.PNG)
