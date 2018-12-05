
export class ZonewiseIncident {
    name: string;
    count: number;
    statusId: string;
    value: number;
}
export class ZonewiseIncidentResponse {
    name: string;
    series: ZonewiseIncident[];
}

export class ZonewiseIncidentChart {
    name: string;
    series: ZonewiseIncidentValue[];
}
export class ZonewiseIncidentValue {
    name: string;
    value: number
}