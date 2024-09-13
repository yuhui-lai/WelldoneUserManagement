﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WUM.Lib.Models.JWT
{
    public class JWTConfig
    {
        /// <summary>
        /// 發行者
        /// </summary>
        /// <value></value>
        public string Issuer { set; get; }

        /// <summary>
        /// 加密金鑰
        /// </summary>
        /// <value></value>
        public string SignKey { set; get; }

        /// <summary>
        /// 設置Token存活多久(分鐘)
        /// </summary>
        /// <value></value>
        public int ExpireDateTime { set; get; }
    }
}
