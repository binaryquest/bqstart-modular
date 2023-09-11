﻿using BinaryQuest.Framework.ModularCore.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System;

namespace BinaryQuest.Framework.ModularCore
{
    public static class ApplicationBuilderExtension
    {
        public static IBQBuilder UseBQAdmin<TContext>(this IApplicationBuilder builder) where TContext : DbContext
        {
            return new Implementation.BQBuilder<TContext>(builder);
        }

    }
}
