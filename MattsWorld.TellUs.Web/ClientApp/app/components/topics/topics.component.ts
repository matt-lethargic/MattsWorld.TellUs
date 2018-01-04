import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'topics',
    templateUrl: './topics.component.html'
})
export class TopicsComponent {
    public topics: ITopic[];

    constructor(http: Http, @Inject('API_URL') apiUrl: string) {
        http.get(apiUrl + 'topics').subscribe(result => {
            this.topics = result.json() as ITopic[];
        }, error => console.error(error));
    }
}

interface ITopic {
    name: string;
    type: string;
}
