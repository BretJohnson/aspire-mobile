// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.

namespace AspireMobile.SettingsGenerator;

public class Setting(string name, string value) : IComparable<Setting>
{
    public string Name { get; } = name;
    public string Value { get; } = value;

    public int CompareTo(Setting? other) =>
        other is null ? 1 : Name.CompareTo(other.Name);
}
