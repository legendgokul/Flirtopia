import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs";
import { PaginationResult } from "../_CustomModels/Pagination";

export function  getPaginationHeaders(
    pageNumber: number,
    pageSize: number
  ): HttpParams {
    //create a model to pass as HeaderParams
    var Params = new HttpParams();
    Params = Params.append('pageNumber', pageNumber); //current page number
    Params = Params.append('pageSize', pageSize); //item per page
    return Params;
  }

  export function getPaginationResult<T>(url: string, params: HttpParams, http: HttpClient) {
    const paginationResult: PaginationResult<T> = new PaginationResult<T>();
    return http.get<T>(url, { observe: 'response', params: params }).pipe(
      map((response) => {
        if (response.body) {
          paginationResult.result = response.body;
        }
        const pagination = response.headers.get('pagination');

        if (pagination) {
          paginationResult.pagination = JSON.parse(pagination);
        }
        return paginationResult;
      })
    );
  }
