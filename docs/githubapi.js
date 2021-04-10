var api_url="https://api.github.com/repos/hypergamesdev/sss222/releases/latest";
var data;
var name;
async function getapi(url){
	const response=await fetch(url);
	data=await response.json();
	//console.log(data);
	name=data.name;
}
getapi(api_url);

function getLatestReleaseName(){
	name=name.replace("SSS222","");
	return name;
}

function sleep(milliseconds) {
  const date = Date.now();
  let currentDate = null;
  do {
    currentDate = Date.now();
  } while (currentDate - date < milliseconds);
}