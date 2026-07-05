using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TinyUtilities.Roslyn.Extensions {
    public static class SyntaxExtension {
        [Pure]
        public static bool TryFindMethod(this ClassDeclarationSyntax declaration, string name, out MethodDeclarationSyntax method) {
            IEnumerable<MethodDeclarationSyntax> methods = declaration.Members.OfType<MethodDeclarationSyntax>();
            
            foreach (MethodDeclarationSyntax other in methods) {
                if (other.Identifier.Text != name) {
                    continue;
                }
                
                if (other.ParameterList.Parameters.Count > 0) {
                    continue;
                }
                
                method = other;
                return true;
            }
            
            method = null;
            return false;
        }
        
        [Pure]
        public static bool IsHaveUsing(this CompilationUnitSyntax syntax, string targetNamespace) {
            foreach (UsingDirectiveSyntax usingDirective in syntax.Usings) {
                if (usingDirective.Name.ToString() == targetNamespace) {
                    return true;
                }
            }
            
            return false;
        }
        
        [Pure]
        public static CompilationUnitSyntax AddUsing(this CompilationUnitSyntax syntax, string targetNamespace) {
            NameSyntax nameSyntax = SyntaxFactory.ParseName(targetNamespace);
            UsingDirectiveSyntax usingDirective = SyntaxFactory.UsingDirective(nameSyntax).WithTrailingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed);
            return syntax.AddUsings(usingDirective);
        }
        
        [Pure]
        public static ClassDeclarationSyntax AddInterface(this ClassDeclarationSyntax declaration, string interfaceName) {
            BaseListSyntax baseList = declaration.BaseList;
            SimpleBaseTypeSyntax interfaceType = SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(interfaceName));
            
            if (baseList == null) {
                return declaration.WithBaseList(SyntaxFactory.BaseList(SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(interfaceType)));
            }
            
            return declaration.AddBaseListTypes(interfaceType);
        }
        
        [Pure]
        public static ClassDeclarationSyntax InsertInterface(this ClassDeclarationSyntax declaration, string interfaceName, int position) {
            BaseListSyntax baseList = declaration.BaseList;
            SimpleBaseTypeSyntax interfaceType = SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(interfaceName));
            
            if (baseList == null) {
                return declaration.WithBaseList(SyntaxFactory.BaseList(SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(interfaceType)));
            }
            
            SeparatedSyntaxList<BaseTypeSyntax> updatedTypes = baseList.Types.Insert(position, interfaceType);
            return declaration.WithBaseList(baseList.WithTypes(updatedTypes));
        }
        
        [Obsolete("Can`t use without parameters!", true)]
        public static bool TryFindAnyPlace(this SeparatedSyntaxList<BaseTypeSyntax> types, out int placeId) {
            placeId = -1;
            return false;
        }
        
        public static bool TryFindAnyPlace(this SeparatedSyntaxList<BaseTypeSyntax> types, out int placeId, params string[] targets) {
            string[] typeNames = new string[types.Count];
            
            for (int typeId = 0; typeId < types.Count; typeId++) {
                typeNames[typeId] = types[typeId].ToString();
            }
            
            placeId = -1;
            
            for (int targetId = targets.Length - 1; targetId >= 0; targetId--) {
                string target = targets[targetId];
                
                for (int typeId = 0; typeId < typeNames.Length; typeId++) {
                    if (typeNames[typeId].Equals(target)) {
                        placeId = typeId;
                        return true;
                    }
                }
            }
            
            return false;
        }
        
        [Pure]
        public static bool IsHaveParentClass(this ClassDeclarationSyntax declaration, SemanticModel semantic) {
            BaseListSyntax baseList = declaration.BaseList;
            
            if (baseList != null && baseList.Types.Count > 0) {
                ITypeSymbol symbol = semantic.GetTypeInfo(baseList.Types[0].Type).Type;
                return symbol != null && symbol.TypeKind != TypeKind.Interface;
            }
            
            return true;
        }
        
        [Pure]
        public static bool IsHaveInterface<T>(this T symbol, string name) where T : INamedTypeSymbol {
            foreach (INamedTypeSymbol current in symbol.AllInterfaces) {
                if (current.Name == name) {
                    return true;
                }
            }
            
            return false;
        }
    }
}