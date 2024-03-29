﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.EntityLayer.Messages
{
    public enum ErrorMessageCode
    {
        UserNameAlreadyExist = 101,
        EmailAlreadyExist = 102,
        UserIsNotActive = 151,
        UsernameOrPassWrong = 152,
        CheckYourEmail = 153,
        UserAlreadyActive = 154,
        ActivateIdDoesNotExist = 155,
        UserNotFound = 156,
        ProfileCouldNotUpdated = 157,
        UserCouldNotRemove = 158,
        UserCouldNotFind = 159,
        UserCouldNotInserted = 160,
        UserCouldNotUpdated = 161,
        UserCouldNotDeleted = 162,
        UserCouldNotLocked = 163,
        UserCouldNotUnLocked = 164,
        UserCouldNotActivated = 165,
        UserCouldNotUpdatedPassword = 166,
        UserCouldNotUpdatedRole = 167,
        UserCouldNotUpdatedActive = 168


    }
}
