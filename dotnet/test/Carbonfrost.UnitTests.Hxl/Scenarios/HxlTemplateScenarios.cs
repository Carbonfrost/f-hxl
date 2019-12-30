//
// - HxlTemplateScenarios.cs -
//
// Copyright 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Core.Runtime;

namespace Scenarios {

    public class HxlTemplateScenarios {

        public void render_hxl_template_to_text() {
            // Reads in an HTML file, executes the HXL commands
            // within to generate HTML text
            string text = HxlTemplate.RenderText("C:/myfile.html");
        }

        public void render_hxl_template_to_file() {
            // Reads in an HTML file, executes the HXL commands
            // within to generate HTML file
            HxlTemplate.Render("C:/input.html", "C:/output.html");
        }

        public void load_hxl_template_from_uri() {
            HxlTemplate template = HxlTemplate.FromSource(new Uri("http://example.com/index.xl.html"));
            string output = template.TransformText();
        }

        public void load_hxl_template_from_assembly() {
            // uses factory, HxlTemplateUsage
            HxlTemplate template = HxlTemplate.FromName(typeof(object).GetTypeInfo().Assembly, "MyTemplate");
            IEnumerable<KeyValuePair<string, object>> variables =
                Properties.FromValue(new { greeting = "Hello, World" });

            template.Transform("C:/output.html", variables);
        }

        public void save_hxl_template_results_to_stream() {
            HxlTemplate template = HxlTemplate.Parse("<html><body>$greeting</body></html>");
            IEnumerable<KeyValuePair<string, object>> variables =
                Properties.FromValue(new { greeting = "Hello, World" });

            FileStream fs = new FileStream("/output.html", FileMode.Create);
            template.Transform(fs, variables);
        }

        public void generate_hxl_runtime_template_source() {
            // Reads in an HTML file, generates a source code
            // file that will generate the output (T4 style template)
            HxlTemplate template = HxlTemplate.FromStream(Stream.Null);
            string source = template.GenerateSource();
        }

    }
}
