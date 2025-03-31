import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddbookModalComponent } from './addbook-modal.component';

describe('AddbookModalComponent', () => {
  let component: AddbookModalComponent;
  let fixture: ComponentFixture<AddbookModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddbookModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddbookModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
