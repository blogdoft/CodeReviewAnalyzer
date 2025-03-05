import {
    AfterViewInit,
    Component,
    ElementRef,
    EventEmitter,
    Input,
    Output,
    ViewChild
} from '@angular/core';
import {
    AutoComplete,
    AutoCompleteDropdownClickEvent,
    AutoCompleteLazyLoadEvent,
    AutoCompleteModule,
    AutoCompleteSelectEvent
} from 'primeng/autocomplete';
import { AutoFocusModule } from 'primeng/autofocus';
import { IftaLabelModule } from 'primeng/iftalabel';

@Component({
    selector: 'app-paginated-lookup',
    imports: [AutoCompleteModule, AutoFocusModule, IftaLabelModule],
    templateUrl: './paginated-lookup.component.html',
    styleUrl: './paginated-lookup.component.scss'
})
export class PaginatedLookupComponent implements AfterViewInit {
    @Input()
    public lookupOptions!: LookupOptions;

    @Input()
    public suggestions!: any[];

    @Input()
    public isLoading: boolean = false;

    @Input()
    public pageResponse!: PageResponse;

    @Input()
    public fieldKey!: any;

    @Input()
    public focus: boolean = false;

    @Output()
    public fieldKeyChange: EventEmitter<any> = new EventEmitter<any>();

    @Output()
    public onLoadData: EventEmitter<LazyQuery> = new EventEmitter<LazyQuery>();

    @Output()
    public onSelect: EventEmitter<AutoCompleteSelectEvent> =
        new EventEmitter<AutoCompleteSelectEvent>();

    @Output()
    public onClear: EventEmitter<Event | undefined> = new EventEmitter<
        Event | undefined
    >();

    @ViewChild('autocomplete', { read: ElementRef }) autoElRef!: ElementRef;
    @ViewChild('autocomplete') private autoComplete!: AutoComplete;

    private lastQuery: string = '';
    private pageSize: number = 0;

    ngAfterViewInit(): void {
        this.applyFocus();
    }

    public clear() {
        this.autoComplete.clear();
    }

    public applyFocus() {
        if (this.focus) {
            setTimeout(() => {
                const input =
                    this.autoElRef.nativeElement.querySelector('input');
                if (input) {
                    input.focus();
                }
            }, 0);
        }
    }

    protected onSelectItem($event: AutoCompleteSelectEvent) {
        this.fieldKey = $event.value[this.lookupOptions.dataKey];
        this.fieldKeyChange.emit(this.fieldKey);
        this.onSelect.emit($event);
    }

    protected onClearSelection($event: Event | undefined) {
        this.fieldKey = null;
        this.fieldKeyChange.emit(this.fieldKey);
        this.onClear.emit($event);
    }

    protected onCompleteMethod(event: AutoCompleteDropdownClickEvent): void {
        this.lastQuery = event.query;
        this.pageResponse.currentPage = 0;
        this.onLoadData.emit({
            page: 0,
            size: this.pageSize,
            params: event.query,
            lazyCall: false
        });
    }

    protected onLazyLoad(event: AutoCompleteLazyLoadEvent): void {
        const page = this.pageResponse.currentPage + 1;
        if (event.last != this.suggestions.length) return;

        if (page > this.pageResponse.totalPages) return;

        if (this.isLoading) return;

        this.onLoadData.emit({
            page: page,
            size: this.pageSize,
            params: this.lastQuery,
            lazyCall: true
        });
    }
}

export interface LazyQuery {
    params: any;
    page: number;
    size: number;
    order?: string;
    lazyCall: boolean;
}

export interface PageResponse {
    totalItems: number;
    totalPages: number;
    currentPage: number;
}

export interface LookupOptions {
    label: string | null | undefined;
    placeholder: string;
    dataKey: string;
    optionLabel: string;
    inputId: string | undefined;
}
