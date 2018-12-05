export class CustomSearchData {
    searchkeydisplayname: string;
    searchkey: string;
    controltype: string; // text, select
    controlvalues: CustomSearchSubSelectionValues[];
}
export class CustomSearchDataMain {
    selected: string[];
    fieldlist: CustomSearchData[];
}

export class CustomSearchSubSelectionValues {
    displaytext: string;
    value: any;
}

export class SearchFilter {
    searchkey: string;
    searchvalue: string;
}