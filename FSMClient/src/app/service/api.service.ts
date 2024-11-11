import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';


export interface Technician {
  id: number;
  name: string;
  email: string;
  location: string;
  latitude: number;
  longitude: number;
  isAvailable: boolean;
}

@Injectable({
  providedIn: 'root'
})

export class ApiService {
  http = inject(HttpClient);

 // getTechnicians(): Observable<Technician[]> {

  }

