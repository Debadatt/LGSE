export class UserwiseIncident {
    name: string;
    value: number;
    statusId: string;
}
export class UserwiseIncidentResponse {
    name: string;
    series: UserwiseIncident[];
}
export class UserwiseIncidentChart {
    name: string;
    series: UserwiseIncidentValue[];
}
export class UserwiseIncidentValue {
    name: string;
    value: number
}

export class UserwiseChartrequest {
    CellName: string;
    IncidentId: string;

}