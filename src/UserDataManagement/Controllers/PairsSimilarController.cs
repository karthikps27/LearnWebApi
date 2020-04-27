using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using Framework;
using Models;

namespace Controllers
{
    [Produces("application/json")]
    [Route("api/pairsSimilarElement")]
    public class PairsSimilarController
    {
        [HttpPost]
        public Object FindSimilarPairElements([FromBody] SimilarPairsRequest requestContent)
        {
            int count = 0;
            dynamic result = new ExpandoObject();
            using(var framework = new SimilarPairsFramework(requestContent.arrayCount)) 
            {
                framework.GenerateRandomArray();
                Array.Sort(framework.Array);
                for(int i = 0, j = 0; i < framework.ArrayLength - 1; i++) 
                {
                    if(framework.Array[i] == framework.Array[i+1] - 1) 
                    {
                        if(i == 0)
                            count++;
                        else
                            count += i-j;
                    }
                    else
                    {
                        j = i;
                    }
                }                                
                result.Array = framework.Array;
                result.Count = count;
            }            
            return result;
        }
    }
}