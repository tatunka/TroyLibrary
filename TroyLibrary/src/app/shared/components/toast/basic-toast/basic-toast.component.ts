import { Component } from '@angular/core';
import { ToastService } from '../toast-service';

@Component({
  selector: 'app-basic-toast',
  standalone: false,
  templateUrl: './basic-toast.component.html',
  styleUrl: './basic-toast.component.css'
})
export class BasicToastComponent {
  constructor(public toastService: ToastService) {}
}
