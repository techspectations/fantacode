<?php

/**
 * Created by PhpStorm.
 * User: ss
 * Date: 12/11/16
 * Time: 10:49 AM
 */

/**
 * Class UserModel
 * @property InterestModel $InterestModel
 */
class UserModel extends CI_Model
{

    public function fbLogin($id_token) {
        $data = (object)array(
            'IStatus' => (object) array(
                'StatusCode' => '',
                'Status' => 'Failed to login'
            ),
            'data' => false
        );

        if($id_token == null) {
            throw new PixelRequestException('1L201| No token specified.');
        }

        //verify the token
        $this->load->library('guzzle');
        $endpoint = 'https://graph.facebook.com/me?fields=id,name&access_token='. $id_token ;

        $client = new \GuzzleHttp\Client();
        $res = $client->request(
            'GET',
            $endpoint
        );

        if($res->getStatusCode() != 200) {
            throw new PixelRequestException('1L201| Failed to verify the Facebook access token.');
        }

        $body = $res->getBody();
        $body = json_decode($body);
        $fb_id = $body->id;
        $name = $body->name;



        $this->load->database();
        $sql = "INSERT INTO users(id, name, fb_user_id) VALUES (NULL, ?, ?)";
        $query = $this->db->query($sql, array(
            $name,
            $fb_id
        ));

        $sql = "SELECT LAST_INSERT_ID() AS id";
        $query = $this->db->query($sql, array());
        $res = $query->result();
        $id = $res[0] -> id;

        $this->InterestModel->postScore($id, 3, 4, 3, 4, 2);

        $data->IStatus->Status = 'Login Successful.';
        $data->IStatus->StatusCode = '1L100';

        $data->data = (object) array(
            'id' => $id,
            'name' => $name
        );

        return $data;
    }

    public function getPages($id_token, $id) {
        $data = (object)array(
            'IStatus' => (object) array(
                'StatusCode' => '',
                'Status' => 'Failed to login'
            ),
            'data' => false
        );

        if($id_token == null) {
            throw new PixelRequestException('1L201| No token specified.');
        }

        $sql = "SELECT * FROM users WHERE id = ?";
        $query = $this->db->query($sql, array($id));
        $res = $query->result();

        if($query->num_rows() < 1)
            throw new PixelRequestException('1L201| Invalid user.');

        $fb_id = $res[0]->fb_user_id;

        //verify the token
        $this->load->library('guzzle');

        $client = new \GuzzleHttp\Client();

        $ret = array();
        $next = true;

        $endpoint = 'https://graph.facebook.com/v2.8/'.$fb_id.'/likes?access_token='. $id_token ;

        while ($next) {
            $res = $client->request(
                'GET',
                $endpoint
            );

            if($res->getStatusCode() != 200) {
                throw new PixelRequestException('1L201| Failed to verify the Facebook access token.');
            }

            $body = $res->getBody();
            $body = json_decode($body);

            $pages = $body->data;
            foreach ($pages as $page) {
                //process here
                //echo $page->name ."\r\n";
                echo $this->getPageInfo($page->id, $id_token);
            }

            if(isset($body->paging->next)) {
                $endpoint = $body->paging->next;
                echo $endpoint;
                $next = true;
            }
            else $next = false;
        }

        return $ret;

        /*$fb_id = $body->id;
        $name = $body->name;


        $data->InfStatus->Status = 'Login Successful.';
        $data->InfStatus->StatusCode = '1L100';

        $data->data = (object) array(
            'id' => $id,
            'name' => $name
        );

        return $data;*/
    }

    public function getPageInfo($page_id, $token) {
        $this->load->library('guzzle');
        $endpoint = 'https://graph.facebook.com/v2.8/'.$page_id . '?access_token=' . $token ;

        $client = new \GuzzleHttp\Client();
        $res = $client->request(
            'GET',
            $endpoint
        );

        if($res->getStatusCode() != 200) {
            return '-';
        }

        $body = $res->getBody();
        //$body = json_decode($body);

        return $body;
    }

    public function search($userId, $term) {
        $url = 'https://api.projectoxford.ai/luis/v2.0/apps/eb850375-99e7-4fb7-961e-4e40155fdfa4?subscription-key=a2219454dda54ab8bab332ea46b9c946&q=' . $term;

        $this->load->library('guzzle');
        $endpoint = $url ;

        $client = new \GuzzleHttp\Client();
        $res = $client->request(
            'GET',
            $endpoint
        );

        if($res->getStatusCode() != 200) {
            throw new PixelRequestException('1L201| Request failed.');
        }

        $body = $res->getBody();
        $body = json_decode($body);

 /*       {
            "query": "Get News that I am interested on",
  "topScoringIntent": {
            "intent": "GetNews",
    "score": 0.999998569
  },
  "entities": [
    {
        "entity": "interested",
      "type": "interest",
      "startIndex": 19,
      "endIndex": 28,
      "score": 0.54080683
    }
  ]
}*/
        $sql = "SELECT * FROM users WHERE id = ?";
        $query = $this->db->query($sql, array(
            $userId
        ));

        if($query->num_rows() == 0) throw new PixelArgumentException('1L300|');
        $res = $query->result()[0];
        $name = $res->name;

        if($body->topScoringIntent != 'none' && $body->topScoringIntent->intent == 'GetNews') {
            if(count($body->entities) == 0) {
                $news = $this->getNews(null);
                return $news;
            }
            else {
                $news = $this->getNews($body->entities[0]->entity);
                return $news;
            }
        }
        else {
            $string = "Hi " . $name . ",\r\nI am your personal news assistant.\r\nYou may ask the following questions : \r\n"
                . "1. News you are interested in.\r\n"
                . "2. News based on different categories (business, sports...).\r\n"
                . "3. Latest news.\r\n"
            ;

            $data = (object)array(
                'IStatus' => (object) array(
                    'StatusCode' => '1F100',
                    'Status' => 'Got score successfully'
                ),
                'data' => (object) array(
                    'type' => 'p',
                    'message' => $string
                )
            );
            return $data;
        }

        return null;
    }

    public function getNews($entity){
        if($entity=="latest" || $entity == null){
            return $this->InterestModel->getLatestNews();
        }else if($entity=="interested"){
            return $this->InterestModel->getInterestedNews();
        }
        else {
            return $this->InterestModel->getNews($entity);
        }

    }

}