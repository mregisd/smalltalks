﻿using SmallTalks.Core.Models;
using SmallTalks.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmallTalks.Core.Services
{
    public class LocalSourceProviderService : ISourceProviderService
    {
        public SourceProvider GetSourceProvider()
        {
            return new SourceProvider
            {
                Intents = "Resources.intents.json",
                StopWords = "Resources.stopwords.txt",
                CurseWords = "Resources.cursewords.txt",
                SourceType = SourceProviderType.Local
            };
        }
    }
}
