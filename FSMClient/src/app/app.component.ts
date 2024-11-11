import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,NgFor],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  http = inject(HttpClient);
  title = 'FSMClient';
  technicians: any;
  serviceRequests: any;

  ngOnInit(): void {
    this.getTechnicians();
    this.getServiceRequests();
  }
  getTechnicians(){
    this.http.get('http://localhost:5003/api/technicians').subscribe({
      next: response => this.technicians = response,
      error: error => console.log(error),
      complete: () => console.log('Request completed')
    })
  }

  getServiceRequests(){
    this.http.get('http://localhost:5003/api/servicerequest').subscribe({
      next: response => this.serviceRequests = response,
      error: error => console.error(error),
      complete: () => console.log('Got servicerequests')
    })
  }


   
    assignedTasks = [
      { id: 1, technicianName: 'Technician A' }
    ];
   
    /*serviceRequests = [
      { id: 1, description: 'Request 1', location: 'Location 1', latitude: 40.7128, longitude: -74.0060 },
      { id: 2, description: 'Request 2', location: 'Location 2', latitude: 34.0522, longitude: -118.2437 }
    ];*/
   
    assignTask() {
      console.log('Assign Task button clicked');
      // Logic for assigning tasks
    }
    unassignTask() {
      console.log('Assign Task button clicked');
      // Logic for assigning tasks
    }
}
