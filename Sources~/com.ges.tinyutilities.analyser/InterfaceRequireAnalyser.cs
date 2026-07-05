using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using TinyUtilities.Roslyn.Extensions;

namespace TinyUtilities.Roslyn {
    public abstract class InterfaceRequireAnalyser : DiagnosticAnalyzer {
        protected abstract DiagnosticDescriptor _rule { get; }
        protected abstract string _methodName { get; }
        protected abstract string _interfaceName { get; }
        
        public override void Initialize(AnalysisContext context) {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeClass, SyntaxKind.ClassDeclaration);
        }
        
        private void AnalyzeClass(SyntaxNodeAnalysisContext context) {
            ClassDeclarationSyntax classDeclaration = (ClassDeclarationSyntax)context.Node;
            
            INamedTypeSymbol symbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);
            
            if (symbol == null) {
                return;
            }
            
            if (classDeclaration.TryFindMethod(_methodName, out MethodDeclarationSyntax methodDeclaration) == false) {
                return;
            }
            
            if (symbol.IsHaveInterface(_interfaceName) == false) {
                Diagnostic classDiagnostic = Diagnostic.Create(_rule, classDeclaration.Identifier.GetLocation(), symbol.Name);
                Diagnostic methodDiagnostic = Diagnostic.Create(_rule, methodDeclaration.Identifier.GetLocation(), symbol.Name);
                
                context.ReportDiagnostic(classDiagnostic);
                context.ReportDiagnostic(methodDiagnostic);
            }
        }
    }
}