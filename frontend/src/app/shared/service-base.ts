import { HttpParams } from '@angular/common/http';

export abstract class ServiceBase {
  protected httpParams(obj: Record<string, unknown>): HttpParams {
    return Object.getOwnPropertyNames(obj).reduce((params, key) => {
      const valor = obj[key];
      if (valor === null || valor === undefined || valor === '') {
        return params;
      }

      return params.set(key, String(valor));
    }, new HttpParams());
  }
}
