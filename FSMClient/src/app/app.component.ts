import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,NgFor,FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  http = inject(HttpClient);
  toastr = inject(ToastrService);

  title = 'FSMClient';
  technicians: any;
  serviceRequests: any;
  assignedTasks: any;
  serviceRequestId: number | undefined;

  ngOnInit(): void {
    this.getTechnicians();
    this.getServiceRequests();
    this.filteredAssignedTasks();
    this.assignTask();
    this.unassignTask();
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
    });
  }

  filteredAssignedTasks(){
    this.http.get('http://localhost:5003/api/scheduler/assigned-tasks').subscribe({
      next: response => {this.assignedTasks = response
        console.log('Assigned Tasks:',this.assignedTasks)
      },
      error: error => console.error(error),
      complete: () => console.log('Got Assigned tasks')
    });
  }
   
    assignTask() {
      if(this.serviceRequestId){

        console.log('Assign task with id:', this.serviceRequestId);
        this.http.post(`http://localhost:5003/api/scheduler/assign-technician/${this.serviceRequestId}`,{})
        .subscribe({
          next: response => {
            this.toastr.success('Task assigned successfully');
            this.filteredAssignedTasks(); //Refresh assigned tasks
          },
          error: error => this.toastr.error('Error assigning task',error)
        });
      }
      else{
        console.warn('Servicerequest ID is required');
      }
    }
    unassignTask() {
      if(this.serviceRequestId){

        console.log('Unassign task with id:', this.serviceRequestId);
        this.http.post(`http://localhost:5003/api/scheduler/unassign-technician/${this.serviceRequestId}`,{})
        .subscribe({
          next: response => {
            
            this.filteredAssignedTasks(); //Refresh assigned tasks
          },
          error: error => this.toastr.success('Task unassigned')
        });
      }
      else{
        console.warn('Servicerequest ID is required');
      }
      this.filteredAssignedTasks();
    }
}
