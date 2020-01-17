//
// Copyright 2014, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using Carbonfrost.Commons.Instrumentation;

namespace Carbonfrost.Commons.Hxl {

    static class Metrics {

        private static readonly Profiler _profiler = Profiler.FromName("carbonfrost.commons.hxl.metrics");

        public static bool EnableAdvancedParserMetrics {
            get {
                // TODO Add a switch for these
                return true;
            }
        }

        public static IProfilerScope ForTemplateParsing() {
            return _profiler.NewScope("templates");
        }

        internal static void StartParsing(
            this IProfilerScope ps) {
            ps.MarkStart("parserTime");
        }

        internal static void EndParsing(
            this IProfilerScope ps,
            string name,
            int contentLength) {

            ps.MarkEnd();
            ps.AddMetric("templateName", name);
            ps.AddMetric("templateLength", contentLength);
        }

        internal static void StartSourceGenerator(
            this IProfilerScope ps) {
            ps.MarkStart("sourceGeneratorTime");
        }

        internal static void EndSourceGenerator(
            this IProfilerScope ps) {

            ps.MarkEnd();
        }

        internal static void SourceCodeMetrics(
            this IProfilerScope ps,
            bool completelyInlined,
            int domNodeCount,
            int generatedCodeLength,
            int renderIslandCount,
            int variableCount) {

            // TODO Currently unsupported in HTML lib - ps.AddMetric("documentNodeCount", this.SourceDocument.Descendents.Count());
            ps.AddMetric("generatedCodeLength", generatedCodeLength);
            ps.AddMetric("nodeCount", domNodeCount);
            ps.AddMetric("renderIslandCount", renderIslandCount);
            ps.AddMetric("variableCount", variableCount);
            ps.AddMetric("completelyInlined", completelyInlined);
            ps.MarkEnd();
        }

        internal static void TemplateOptimizerStarting(
            this IProfilerScope ps,
            string codeProvider,
            bool usingOptimizer) {
            ps.AddMetric("codeProvider", codeProvider);
            ps.AddMetric("usingOptimizer", usingOptimizer);
        }

        internal static void StartOptimizer(
            this IProfilerScope ps) {
            ps.MarkStart("optimizerTime");
        }

        internal static void EndOptimizer(
            this IProfilerScope ps) {
            ps.MarkEnd();
            // TODO Compute optimizer efficiency
        }

        internal static void StartPreprocessor(
            this IProfilerScope ps) {
            ps.MarkStart("preprocessorTime");
        }

        internal static void EndPreprocessor(
            this IProfilerScope ps) {
            ps.MarkEnd();
        }
    }
}
