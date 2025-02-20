import { Directive, ElementRef, Input } from '@angular/core';

@Directive({
  selector: '[appMathjax]',
  standalone: true,
})
export class MathjaxDirective {
  @Input('appMathjax') mathContent!: string;

  constructor(private el: ElementRef) {}

  ngOnChanges() {
    this.el.nativeElement.innerHTML = this.mathContent;
    if ((window as any).MathJax) {
      (window as any).MathJax.typesetPromise([this.el.nativeElement]);
    }
  }
}
