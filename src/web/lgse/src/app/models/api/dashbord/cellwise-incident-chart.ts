export class CellwiseIncident {
    name: string;
    count: number;
    statusId: string;
    value: number;
}
export class CellwiseIncidentResponse {
    name: string;
    series: CellwiseIncident[];
}
export class CellwiseIncidentChart {
    name: string;
    series: CellwiseIncidentValue[];
}
export class CellwiseIncidentValue {
    name: string;
    value: number
}