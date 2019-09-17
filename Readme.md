This .Net core C# program aim to generate png capture of chrome and crop them based on a json config file.

```json
{
    "chromePath" : "C:\\Users\\Username\\AppData\\Local\\Google\\Chrome\\Application\\chrome.exe",
    "actions": [
        {
            "url" : "https://www.whatever.com",
            "path" : "C:\\Users\\Username\\Desktop\\whatever.png",
            "size" : {
                "x" : 980,
                "y" : 740
            },
            "crop" : {
                "size":  {
                    "x" : 500,
                    "y" : 500
                },
                "topLeftCorner" : {
                    "x" : 200,
                    "y" : 200,
                }
            }
        }
    ]
}
```