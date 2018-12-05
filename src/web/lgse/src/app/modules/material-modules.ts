import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import {
    MatInputModule,
    MatTableModule,
    MatToolbarModule,
    MatPaginatorModule,
    MatMenuModule,
    MatSidenavModule, MatSlideToggleModule,
    MatFormFieldModule,
    MatButtonModule, MatTooltipModule,
    MatTabsModule,
    MatCheckboxModule, MatSelectModule, MatSortModule, MatIconModule,
    MatExpansionModule, MatDialogModule,
    MatListModule, MatDatepickerModule, MatNativeDateModule,
    MatAutocompleteModule, MatRadioModule, MatCardModule, MatProgressBarModule,
    MatBottomSheetModule, MatButtonToggleModule, MatBadgeModule


} from '@angular/material';

@NgModule({
    imports: [
        CommonModule,
        MatBadgeModule,
        MatInputModule, MatTableModule, MatToolbarModule, MatPaginatorModule, MatMenuModule, MatSidenavModule, MatSlideToggleModule, MatFormFieldModule, MatButtonModule, MatTooltipModule,
        MatTabsModule,
        MatCheckboxModule, MatSelectModule, MatSortModule, MatIconModule,
        MatExpansionModule, MatDialogModule,
        MatListModule, MatDatepickerModule, MatNativeDateModule,
        MatAutocompleteModule, MatRadioModule, MatCardModule, MatProgressBarModule,
        MatBottomSheetModule, MatButtonToggleModule
    ],
    exports: [
        MatInputModule, MatTableModule, MatToolbarModule, MatPaginatorModule, MatMenuModule, MatSidenavModule, MatSlideToggleModule, MatFormFieldModule, MatButtonModule, MatTooltipModule,
        MatTabsModule,
        MatBadgeModule,
        MatCheckboxModule, MatSelectModule, MatSortModule, MatIconModule,
        MatExpansionModule, MatDialogModule,
        MatListModule, MatDatepickerModule, MatNativeDateModule,
        MatAutocompleteModule, MatRadioModule, MatCardModule, MatProgressBarModule,
        MatBottomSheetModule, MatButtonToggleModule
    ]
})


export class MaterialModules {
    static forRoot() {
        return {
            ngModule: MaterialModules,
        };
    }
}
