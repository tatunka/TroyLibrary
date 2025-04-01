import { Component, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CreateReviewRequest, GetReviewsResponse } from '../../models/models';
import { ReviewService } from '../../services/review.service';
import { ToastService } from '../toast/toast-service';

@Component({
  selector: 'app-review-modal',
  standalone: false,
  templateUrl: './review-modal.component.html',
  styleUrl: './review-modal.component.css'
})
export class ReviewModalComponent {

  constructor(
    private activeModal: NgbActiveModal, 
    private reviewService: ReviewService,
    private toast: ToastService,
  ) {}

  @Input() public bookId?: number;
  @Input() public callback: () => void = () => null;

  form = new FormGroup({
    rating: new FormControl<number>(0, Validators.required),
    text: new FormControl<string>('', Validators.required)
  });

  close() {
    this.activeModal.close();
  }

  postReview() {
    const request: CreateReviewRequest = {
      bookId: this.bookId as number,
      rating: this.form.value.rating as number,
      text: this.form.value.text as string,
    }
    this.reviewService.CreateReview(request).subscribe({
      next: (response: GetReviewsResponse) => {
        if (response.reviews?.length > 0) {
          this.toast.showSuccess('Review posted!');
          this.callback();
        }
        else {
          this.toast.showError('Unable to post review');
        }
      },
      error: (error) => {
        this.toast.showError('Unable to post review');
        console.log(error.message);
      }
    });
  }
}
