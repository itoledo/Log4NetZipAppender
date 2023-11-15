#region Apache License
//
// Licensed to the Apache Software Foundation (ASF) under one or more 
// contributor license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright ownership. 
// The ASF licenses this file to you under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with 
// the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using log4net.Util;
using System;
using System.IO;
using System.IO.Compression;


namespace Log4Net.Appender.Misc
{

    public class CompressorRollingFileAppender : RollingFileAppender
    {

        private readonly static Type declaringType = typeof(CompressorRollingFileAppender);

        private static readonly string EXT_COMPRESSED = ".zip";

        /* BEGIN: log4net improvement - by Ferenc HORVATH: to compress log file after rolling */
		override protected void RollFile(string fromFile, string toFile) 
		{
            base.RollFile(fromFile, toFile);

			if (FileExists(toFile))
			{
				try
				{
                    string compressedFileName = toFile + EXT_COMPRESSED;
					LogLog.Debug(declaringType, "Compressing file [" + toFile + "] -> [" + compressedFileName + "]");
                    FileInfo previousFileInfo = new FileInfo(toFile);
                    using (FileStream previousFileSteram = previousFileInfo.OpenRead())
                    using (FileStream compressedFileSteram = System.IO.File.Create(compressedFileName))
                    using (ZipArchive zipArchive = new ZipArchive(compressedFileSteram, ZipArchiveMode.Create))
                    {
                        var entry = zipArchive.CreateEntry(Path.GetFileName(toFile) + ".log", CompressionLevel.Fastest);
                        using (var stream = entry.Open())
                        {
                            previousFileSteram.CopyTo(stream);
                        }
                        LogLog.Debug(declaringType,
                            "Compressed " + previousFileInfo.Name +
                            " from " + previousFileInfo.Length.ToString() +
                            " to " + compressedFileSteram.Length.ToString() + " bytes.");
                    }
                    LogLog.Debug(declaringType, "Deleting file [" + toFile + "]");
                    previousFileInfo.Delete();
				}
				catch(Exception moveEx)
				{
					ErrorHandler.Error("Exception while compressing file [" + toFile + "]", moveEx);
				}
			}
			else
			{
				LogLog.Warn(declaringType, "Cannot compress file [" + toFile + "]. File does not exist!");
			}
		}
	}
}
