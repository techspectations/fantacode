import nltk
import json
from pprint import pprint

with open('data.json', 'r') as fp:
	data = json.load(fp)
	


	
pprint(data)
classifier = nltk.NaiveBayesClassifier.train(data)