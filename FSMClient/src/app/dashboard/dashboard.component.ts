import { Component, inject, OnInit } from '@angular/core';
import { ApiService } from '../service/api.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})


export class DashboardComponent implements OnInit {
  apiService = inject(ApiService)
  http = inject(HttpClient)
  technicians: any;

  ngOnInit(): void {
    this.getTechnicians();
  }

  getTechnicians(){
    this.http.get('http://localhost:5003/api/technicians').subscribe({
      next: data =>this.technicians = data,
      error: error => console.log(error),
      complete: () => console.log('Request completed')
    })
  }
}
