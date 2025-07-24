// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using UnityEngine;

namespace TinyUtilities {
    public sealed class FileLog {
        private readonly string _pathToLog;
        
        public FileLog(string fileName, params string[] labels) {
            try {
                _pathToLog = Path.Combine(Application.persistentDataPath, fileName);
                
                if (File.Exists(_pathToLog) == false) {
                    StringBuilder logResult = new StringBuilder();
                    
                    if (labels != null) {
                        for (int i = 0; i < labels.Length; i++) {
                            logResult.Append(labels[i]);
                            logResult.Append('\t');
                        }
                    } else {
                        logResult.Append(fileName);
                    }
                    
                    logResult.AppendLine();
                    WriteLog(logResult.ToString(), FileMode.OpenOrCreate);
                }
            } catch (Exception exception) {
                Debug.LogError(exception);
            }
        }
        
        [Obsolete("Cant add empty message!")]
        public void Add() => Debug.LogError("Can't add empty message!");
        
        public void Add([NotNull] params string[] values) {
            try {
                StringBuilder logResult = new StringBuilder();
                
                for (int i = 0; i < values.Length; i++) {
                    logResult.Append(values[i]);
                    logResult.Append('\t');
                }
                
                logResult.AppendLine();
                
                WriteLog(logResult.ToString());
            } catch (Exception exception) {
                Debug.LogError(exception);
            }
        }
        
        private void WriteLog(string data, FileMode mode = FileMode.Append) {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            using FileStream fileStream = new FileStream(_pathToLog, mode, FileAccess.Write, FileShare.Write, 4096, FileOptions.None);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }
    }
}