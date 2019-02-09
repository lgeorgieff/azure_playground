#r "Newtonsoft.Json"
#r "System.Collections"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Collections.Generic;

public static Dictionary<string, int> getBiGrams(string text) {
    Dictionary<string, int> result = new Dictionary<string, int>();

    for(int pos = 0; pos < text.Length - 1; ++pos) {
        if(!Char.IsLetter(text[pos]) || !Char.IsLetter(text[pos + 1])) continue;
        string key = Char.ToLower(text[pos]).ToString() + Char.ToLower(text[pos + 1]).ToString();

        if(result.ContainsKey(key)) result[key] = result[key] + 1;
        else result.Add(key, 1);
    }

    return result;
}

public static async Task<IActionResult> Run(HttpRequest req, ILogger log) {
    log.LogInformation("C# HTTP trigger function processed a request.");

    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    
    try {
        dynamic jsonValue = JsonConvert.DeserializeObject(requestBody);
        if(!(jsonValue is string)) throw new Exception("JSON value must be of type string");
        return new OkObjectResult(JsonConvert.SerializeObject(getBiGrams(jsonValue), Formatting.Indented));
    } catch (Exception error) {
        log.LogError("C# HTTP trigger function error: " + error.ToString());
        return new BadRequestObjectResult("Please pass a valid JSON string in the request body (" + error.ToString() + ")");
    }
}
