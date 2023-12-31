using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using LotteriaView.Models;
namespace LotteriaView.Controllers
{
  [ApiController]
  public class ChatGPTController : Controller
  {
    [HttpPost]
    [Route("AskChatGPT")]
    public async Task<IActionResult> AskChatGPT([FromBody] string query)
    {
      string chatURL = "https://api.openai.com/v1/chat/completions";
      string apiKey = "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";
      StringBuilder sb = new StringBuilder();

      HttpClient oClient = new HttpClient();
      oClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

      ChatRequest oRequest = new ChatRequest();
      oRequest.model = "gpt-3.5-turbo";

      Message oMessage = new Message();
      oMessage.role = "user";
      oMessage.content = query;

      oRequest.messages = new Message[] { oMessage };

      string oReqJSON = JsonConvert.SerializeObject(oRequest);
      HttpContent oContent = new StringContent(oReqJSON, Encoding.UTF8, "application/json");

      HttpResponseMessage oResponseMessage = await oClient.PostAsync(chatURL, oContent);

      if (oResponseMessage.IsSuccessStatusCode)
      {
        string oResJSON = await oResponseMessage.Content.ReadAsStringAsync();

        ChatResponse oResponse = JsonConvert.DeserializeObject<ChatResponse>(oResJSON);

        foreach (Choice c in oResponse.choices)
        {
          sb.Append(c.message.content);
        }

        HttpChatGPTResponse oHttpResponse = new HttpChatGPTResponse()
        {
          Success = true,
          Data = sb.ToString()
        };

        return Ok(oHttpResponse);
      }
      else
      {
        string oFailReason = await oResponseMessage.Content.ReadAsStringAsync();
        return BadRequest(oFailReason); ;
      }
    }
  }
}
