﻿using BL.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.DTO
{
    public class DTOProject
    {
        public int projectScenario { get; set; }
        public string titel { get; set; }
        public string vraag { get; set; }
        public string extraInfo { get; set; }
        public float bedrag { get; set; }
        public float minBedrag { get; set; }
        public float maxBedrag { get; set; }
        public List<DTOGemeenteCategorie> cats { get; set; }
    }
}