# ScorpiusGE
A Generic Enabler (GE) compatible with the FIWARE ecosystem to support Firebase Cloud Messaging. 

This GE serves to complement the FIWARE ecosystem. 
Namely to allow the ORION subscription's notification to be send by Firebase cloud messaging (FCM) to mobile devices.


## Simple Messages
It is also possible to use the `POST /notifyMessage` endpoint to send simple notifications.
The Request to this endpoint should contain a body with the following structure:
```JSON
{
  "topic": "b3ed663cee243170b55f5f0396905d12",
  "message": "Test with some message"
}
```
Where the `topic` is the topic to which the user wants to send notifications and the `message` is the message to be sent in the data of the notification.

## FCM for ORION subscriptions

Only FCM notification by topic are supported. 

For now only ORION subscriptions with the `"attrsFormat":"keyValues"` are supported. 
That is subscriptions with this format:
``` JSON
{
    "description": "A subscription to send notification by FCM using the scorpius GE",
    "expires": "2040-01-01T14:00:00.00Z",
    "subject": {
        "entities": [
            {
                "idPattern": ".*",
                "type": "questionnaire"
            }
        ],
        "condition": {
            "attrs": ["course"]
        }
    },
    "notification": {
        "attrs":  [
                  ],
        "attrsFormat": "keyValues",
        "http": {
            "url": "http://scorpius/notify"
        }
    }
}
```

Notifications should be send to the `POST /notify` endpoint.

## Configuration of Orion Subscriptions

To define a new subscription path you should complemt the `notification.json` file, with a specific configuration for each type of entity.
There are 4 types of notifications attributes that can be used: 
* `topic`: The topic to which the notification must be sent. (`default=$"{id}"`)

* `message`: A text message to be displayed or analysed by the subscribers of the topic. (`default="New {type}"`)

* `type`: Type of the notification to be used for filtering by clients (`default="{type}"`).
 
* `shouldSend`: expression to validate if the notification should be sent by FCM (`default="true"`) .

None of these values are required, and all of them have a default value;
The default value for each of them can be seen in the list above.

These values can be hard coded or can be expressions that include the attributes of the entity.
An example of a configuration for an entity of the type questionnaire that has the attribute course:
 ```JSON
 {
  "questionnaire": {
    "topic": "{course}",
    "message": "New {type}",
    "type": "new_{type}",
     "shouldSend": "{time_type}==scheduled"
    },
    "course":{},
    "car":{
      "topic":"car_{color}",
      "message": "A new car entity update"
    }
  }
 ```
The notification received by orion should include the attributes used to define the path of the Scorpius notification, 
as such be aware of the filter in the notification.attrs field  when creating the ORION's subscriptions.

Additionally, there could be an empty definiton, as for the example for the `"course"` entity, in that case all of the default values will be assumed.


All of the attributes are straight forward apart from the `shouldSend` which support expressions that can be validated to boolean.
The expressions can be hard coded or can use attributes of the entity to choose if the notification is sent or not. 
For instance if we only want to send notifications about can entities that are red:
```JSON
{
   "car":{
      "shouldSend":"{color}==red"
   }
}
```
For now this is function only support boolean values (e.g., "true", "false" or attribute mapping that gives those values) 
or expressions of equality (`==`) and inequality (`!=`).
If the entity type contains a boolean attribute mapping can also be done directly:
```JSON
{
   "car":{
      "shouldSend":"{booleanAttr}"
   }
}
```

Negation of attributes (`!`) is not implemented yet as such you the inequality or equality examples should be used `"shouldSend": "{booleanAttr}==false"` or `"shouldSend": "{booleanAttr}!=true"`

This is a smaller use case since ORION subscriptions already allow the filtering of subscription's notification by expressions.

Since The notification definition is made based on the entity type, only one notification per entity type is allowed.

If you required several distinct notification for the same entity type, you should deploy more instances of this GE (each with a ORION subscription). 

## Run with as a .NET6 service

### Configue files
To configure the service you need to replace the firebase-admin.json with your firebase crednetials file.
If the file as a different name change the mapping in th appsettings.json file, under the FirebaseCM.AppConfig to your file path.

Additionally you need to define your notifications in the notification.json file;

The server runs by default in the port 80(http) and 443(https).
But you can change this configurations in the 
### Install .NET6
To run the Scorpius GE you need to have .Net6 installed. 
.NET6 is a powerfull cross-platform tool that allows the creation of several types of cross-platform applications, as well as microservices, MVC servers and web pages.
To download and install the .NET6 to your system follow the instruction from [microsoft website](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) 

If you have Visual studio 2022 or later installed you should be able to also download and install through the VS Installer. 

### Run system
clone the repository:
```
git clone https://github.com/jmSfernandes/ScorpiusGE.git
```

move to the project directory:
```
cd ScorpiusGE\Scorpius
```

build the project:
```
dotnet build
```

Execute project by either running the .exe or the .dll:
```
//dll 
dotnet bin\Debug\net6\Scorpius.dll

// windows executable
bin\Debug\net6\Scorpius.exe
```


## DOCKER 

To be added... 
