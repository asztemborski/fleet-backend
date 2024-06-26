﻿namespace Thrive.Modules.Identity.Application.Contracts;

public interface IValueHasher
{
    string Hash(string value);
    bool Verify(string valueHash, string value);
}