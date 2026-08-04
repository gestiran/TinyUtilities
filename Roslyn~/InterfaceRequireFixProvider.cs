using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using TinyUtilities.Roslyn.Extensions;

namespace TinyUtilities.Roslyn {
    public abstract class InterfaceRequireFixProvider : CodeFixProvider {
        protected abstract string _title { get; }
        protected abstract string _key { get; }
        protected abstract string _namespace { get; }
        
        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;
        
        public override async Task RegisterCodeFixesAsync(CodeFixContext context) {
            SyntaxNode root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            
            if (root == null) {
                return;
            }
            
            Diagnostic diagnostic = context.Diagnostics[0];
            TextSpan diagnosticSpan = diagnostic.Location.SourceSpan;
            SyntaxNode parent = root.FindToken(diagnosticSpan.Start).Parent;
            
            if (parent == null) {
                return;
            }
            
            ClassDeclarationSyntax declaration = parent.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            
            if (declaration == null) {
                return;
            }
            
            CodeAction codeAction = CodeAction.Create(_title, cancellation => AddInterfaceAsync(context.Document, declaration, cancellation), _key);
            context.RegisterCodeFix(codeAction, diagnostic);
        }
        
        private async Task<Document> AddInterfaceAsync(Document document, ClassDeclarationSyntax declaration, CancellationToken cancellation) {
            CompilationUnitSyntax root = await document.GetSyntaxRootAsync(cancellation) as CompilationUnitSyntax;
            
            if (root == null) {
                return document;
            }
            
            SemanticModel semantic = await document.GetSemanticModelAsync(cancellation);
            
            if (semantic == null) {
                return document;
            }
            
            CompilationUnitSyntax newRoot = root.ReplaceNode(declaration, ApplyFix(declaration, semantic));
            
            if (root.IsHaveUsing(_namespace) == false) {
                newRoot = newRoot.AddUsing(_namespace);
            }
            
            return document.WithSyntaxRoot(newRoot);
        }
        
        protected abstract ClassDeclarationSyntax ApplyFix(ClassDeclarationSyntax declaration, SemanticModel semantic);
    }
}