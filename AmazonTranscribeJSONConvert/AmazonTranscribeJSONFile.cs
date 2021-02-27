using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AmazonTranscribeJSONConvert
{
    class AmazonTranscribeJSONFile
    {
        private String input;

        public String Input
        {
            get { return input; }
            set { 
                input = value;
                this.Parse();
            }
        }
        public List<String> Transcripts = new List<String>();

        public AmazonTranscribeJSONFile(String input)
        {
            this.Input = input;
        }

        private void Parse()
        {
            if (String.IsNullOrEmpty(input)) {
                throw new InvalidOperationException("Cannot parse without input");
            };

            JObject jsonResult = JObject.Parse(input);

            var transcripts = jsonResult["results"]["transcripts"].Children();

            foreach(JToken transcript in transcripts)
            {
                Transcripts.Add(transcript["transcript"].ToString());
            }
            
        }

        public override string ToString()
        {
            return this.Input;
        }
    }
}
