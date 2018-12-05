export class PayloadResponse<T> {
    issuccess: boolean;
    errorhandled: boolean;
    data: T;
    error: PayloadError;
}

export class PayloadError {
    code: number;
}

export class ApiSuccessResponse {
    message: string;
    token: string;
    userId: string;
    username: string;
}

