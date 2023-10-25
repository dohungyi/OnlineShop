export enum GenderType {
    Male = 0,
    Female = 1,
    Other = 2
}

export enum FilterCondition {
    E = 1, // Equal
    NE, // Not equal
    GT, // Greater than
    GE, // Greater than or equal to
    LT, // Less than
    LE, // Less than or equal to
    C, // Contains
    NC,// Not contains
    SW, // Start withs
    NSW, // Not start withs
    EW, // End withs
    NEW, // Not end withs
}

export enum MessageBoxType {
    None = 0,
    Confirm = 1,
    ConfirmDelete = 2,
    Information = 3,
}


export enum GroupBoxFieldType {
    Text = 1,
    Number = 2,
    Date = 3,
    ComboBox = 4,
    CheckBox = 5,
    Image = 6,
    TextArea = 7,
    Link = 8,
    Tag = 9,
    DateTime = 10,
}

export enum FormMode {
    None = 0,
    View = 1,
    Add = 2,
    Update = 3,
}

export enum ExportType {
    All = 1,
    OnScreen = 2,
}
