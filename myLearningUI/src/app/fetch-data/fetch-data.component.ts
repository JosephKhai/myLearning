import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from '../../environments/environment';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
  styleUrls: ['./fetch-data.component.css']
})
export class FetchDataComponent implements OnInit {

  public forecasts?: WeatherForecast[];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getWeatherForecast();
  }

  getWeatherForecast() {
    this.http.get<WeatherForecast[]>(environment.baseUrl + 'api/weatherforecast')
    .subscribe(data => {
      this.forecasts = data;
    }, (error) => {
      console.log(error);
    })
  }

}
