<div align="center">
<img width="150" src="https://github.com/thespragg/Mimic/assets/11881500/0710a707-d946-4922-9877-a48104f9e989"/>
 <h1>Mimic</h1> 
<div align="center"><font size="">A simple approach to API mocking.</font></div>

## Getting Started

Create a json file (or files) defining the endpoints you want mimic to have:

```json
{
    "/health":{
        "method": "GET",
        "response": "{\"name\":\"MyApi\",\"status\":\"Healthy\",\"checks\":{\"Database\":\"Healthy\"}}"
    }
}
```

### Docker

Run it using the following command:

`docker run -d -p 5999:8080 -v /path/to/folder/containing/your/json/file:/endpoints thespragg/mimic:latest````

You will then be able to access the api at `http://localhost:5999`

### Standalone 

Download the latest version https://github.com/thespragg/Mimic/releases

Run it using either:

`./mimic --file path/to/your/json/file` 
`./mimic --directory path/to/folder/containing/json/file` 
