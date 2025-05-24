import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Angular APp';
  fady = 'Fady';
  weathers: Observable<Array<any>> = new Observable<Array<any>>();

  constructor(private http: HttpClient) {
    // this.http.get('http://localhost:7000/weatherforecast').subscribe((data) => {
    //   console.log(data);

    // },
    // (error) => {
    //   console.error('Error fetching data localhost:', error);
    // });
    console.log('Fetching data from localhost');
    this.getData();
  }

  async getData() {
    this.weathers = this.http.get<Array<any>>('https://localhost:7001/weatherforecast',{withCredentials: true});
  }
}
