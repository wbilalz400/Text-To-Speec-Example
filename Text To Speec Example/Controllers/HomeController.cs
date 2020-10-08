using Google.Cloud.TextToSpeech.V1;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Google.Apis.Auth.OAuth2;

using System.Web.Mvc;

namespace Text_To_Speec_Example.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult TextToSpeech()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TextToSpeech(string text, string lang, string voice, double rate)
        {
            try

            {
                TextToSpeechClient client;
                try
                {
                    client = TextToSpeechClient.Create();
                }
                catch (Exception e)
                {
                    var clientBuilder = new TextToSpeechClientBuilder();
                    clientBuilder.CredentialsPath = Server.MapPath("/Views/key.json");
                    client = clientBuilder.Build();

                }

                // Set the text input to be synthesized.
                SynthesisInput input = new SynthesisInput
                {
                    Text = text
                };

                // Build the voice request, select the language code ("en-US"),
                // and the SSML voice gender ("neutral").
                SsmlVoiceGender gender;
                switch (voice)
                {
                    case "0":
                        gender = SsmlVoiceGender.Male;
                        break;
                    case "1":
                        gender = SsmlVoiceGender.Female;
                        break;
                    default:
                        gender = SsmlVoiceGender.Neutral;
                        break;

                }

                VoiceSelectionParams voiceSel = new VoiceSelectionParams
                {
                    LanguageCode = lang,
                    SsmlGender = gender,
                  
                };


                // Select the type of audio file you want returned.
                AudioConfig config = new AudioConfig
                {
                    AudioEncoding = AudioEncoding.Mp3,
                    SpeakingRate = rate,
                    
                    
                    
                };

                // Perform the Text-to-Speech request, passing the text input
                // with the selected voice parameters and audio file type
                var response = client.SynthesizeSpeech(new SynthesizeSpeechRequest
                {
                    Input = input,
                    Voice = voiceSel,
                    AudioConfig = config
                });

                // Write the binary AudioContent of the response to an MP3 file.

                return Content(response.AudioContent.ToBase64());
            }catch (Exception e)
            {
                return Content(e.ToString());
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}