import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasicToastComponent } from './basic-toast.component';

describe('BasicToastComponent', () => {
  let component: BasicToastComponent;
  let fixture: ComponentFixture<BasicToastComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BasicToastComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BasicToastComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
