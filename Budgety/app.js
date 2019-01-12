var budgetController = (function(){
    
    var Income = function(id, description, value,){
        this.id = id;
        this.description = description;
        this.value = value;
        this.percentage = -1;
    }
    
    var Expense = function(id, description, value){
        this.id = id;
        this.description = description;
        this.value = value;
    }
    
    
    var data = {
        allitems: {
            inc: [],
            exp: []
        },
        totals:{
            inc: 0,
            exp: 0,
            sum: 0,
            percentage: -1
        }
    };
    
    var updateExpensesPercentages = function (){     //to be called after deleting and adding items
            if (data.totals.sum > 0){
                data.allitems.exp.forEach(function(e){
                    e.percentage = Math.round( e.value / data.totals.inc * 100);
                }
            );
            }
        }
    
    
    return {
        addItem: function (item){
            var newItem, Id;
            //create id 
            //newId = Id_of_last_item +1 
            
            if (data.allitems[item.type].length === 0){
                Id = 0;
            }
            else {
                Id = data.allitems[item.type][data.allitems[item.type].length-1].id + 1;
            }
            
            
            if (item.type === 'inc'){
                newItem = new Income (Id, item.description, item.value);
                data.totals.sum += item.value;
            }
            else if (item.type ==='exp'){
                newItem = new Expense (Id, item.description, item.value);
                data.totals.sum -= item.value;
            }
            //update the data
            data.allitems[item.type].push(newItem);
            data.totals[item.type] += item.value;
            //percentage? 
            if (data.totals.inc > 0){
                data.totals.percentage = Math.round(100 * data.totals.exp/ data.totals.inc);
            }
            else{
                data.totals.percentage = -1;
            }
            updateExpensesPercentages();
            return Id;
            
        },
        calcBudget: function(){ //realy no need for this method, all its responsiblities can be done in addItem method
            //1. calculate totals: done already before he does it 
            
            //2. calculte the budget or sum, already done :)
            
            // update the percentage, alerady done 
            
            
        },
        
        deleteItem: function(itemId){
            var id, type;
            var stringArr = itemId.split('-');
            type = stringArr[0];
            id = stringArr[1];
            
            data.allitems[type].forEach(function(e,i){
                if (e.id == id ){
                    
                    data.totals[type] -= e.value;
                    data.totals.sum = data.totals.inc - data.totals.exp;
                    if (data.totals.inc > 0){
                    data.totals.percentage = Math.round(100 * data.totals.exp/ data.totals.inc);
                }
                else{
                    data.totals.percentage = -1;
                    }
                    
                        data.allitems[type].splice(i, 1);
                    }});
            updateExpensesPercentages();
        },
        
        
        
        getExpensesPercentages: function(){
            var pers = data.allitems.exp.map(function(element){
                return element.percentage;
            });
            
            return pers;
        },    
            
        getBudget: function(){
            return{
                sum: data.totals.sum,
                totalInc: data.totals.inc,
                totalExp: data.totals.exp,
                per: data.totals.percentage
            }
        },
        
        testData: function(){
            return data;
        }
    };
    
    
})();


var UIController = (function(){
    
    return{
        getInput: function (){
            return{
                des: document.querySelector('.add__description').value,
                type: document.querySelector('.add__type').value,
                value: parseFloat(document.querySelector('.add__value').value)
            };
        },
        addListItem: function(item, id){
            
            var html, elementWhere2Insert;
            
            // Create HTML string with placeholder text
            if (item.type === 'inc'){
                html = '<div class="item clearfix" id="inc-%id%"><div class="item__description">%description%</div><div class="right clearfix"><div class="item__value">+ %value%</div><div class="item__delete"><button class="item__delete--btn"><i class="ion-ios-close-outline"></i></button></div></div></div>' ; 
                
                elementWhere2Insert = '.income__list';
            }
            else if(item.type === 'exp'){
                html =' <div class="item clearfix" id="exp-%id%"><div class="item__description">%description%</div><div class="right clearfix"><div class="item__value">- %value%</div><div class="item__percentage">21%</div><div class="item__delete"><button class="item__delete--btn"><i class="ion-ios-close-outline"></i></button></div></div></div>' ; 
                
                elementWhere2Insert = '.expenses__list';
            }
            // Replace the placeholder text with some actual data
            var newHtml = html.replace('%id%', id);
            newHtml = newHtml.replace('%description%', item.des);
            newHtml = newHtml.replace('%value%', item.value);
            
            // Insert the HTML into the DOM
            
            document.querySelector(elementWhere2Insert).insertAdjacentHTML('beforeend',newHtml);
            
        },
        clearFields: function(){
            var fields = document.querySelectorAll('.add__value , .add__description ');
            var fieldArr ;
            //fieldArr = fieldArr.slice.call(fields);
            fieldArr = Array.prototype.slice.call(fields,0,2);
            fieldArr.forEach(function(current){
               current.value =''; 
            });
            fieldArr[0].focus();
            //document.querySelector('.add__value , .add__description ').value
        },
        
        deleteItem: function(itemId){
            document.querySelector('#'+itemId).remove();
        },
        
        updateBudget: function(budget){
            document.querySelector('.budget__value').textContent = budget.sum;
            document.querySelector('.budget__income--value').textContent = budget.totalInc;
            document.querySelector('.budget__expenses--value').textContent = budget.totalExp;
            //the percentage should only displayed when there is income in the first place
            if (budget.per !== -1 ){
                document.querySelector('.budget__expenses--percentage').textContent = budget.per + '%';
            }
            else {
                document.querySelector('.budget__expenses--percentage').textContent = '%';
            }
        },
        
        updateExpensesPercentages:function(p){
            var listOfExpensesDiv = document.querySelectorAll('.item__percentage');
            
            listOfExpensesDiv.forEach(function(div, index){
                div.textContent = p[index];
            });
            
        },
        
        displayMonth: function(){
            var now = new Date();
            var m = now.getMonth();
            var yearMonths = ['jan','feb','mar','apr','may','jun','jul','aug','sep','oct','nov','dec'];
            document.querySelector('.budget__title--month').textContent = yearMonths[m];
        }
        
    };
    
})();



//third controller to define how they communicate

var Controller = (function(budgetCtrl, UICtrl){
    
    var setEventListeners = function(){
        document.querySelector('.add__btn').addEventListener('click' , ctrlAddItem);
    
    document.addEventListener('keypress',function(e){
        if (e.keyCode == 13){
            ctrlAddItem();
        }
    });
        
    document.querySelector('.container').addEventListener('click',deleteItem);
    }
    
    var updateBudget = function (){
        //1.
        // 2. Return the budget
        var budget = budgetCtrl.getBudget();
        //3. pass it to the ui 
        UICtrl.updateBudget(budget);
    } ;
    
    var ctrlAddItem = function(){
        
        // 1. Get the field input data
        var inputs = UICtrl.getInput();
        
        if (inputs.value > 0){
                // 2. Add the item to the budget controller
            var newItemId = budgetCtrl.addItem(inputs);
            // 3. Add the item to the UI
            UICtrl.addListItem(inputs,newItemId);
            // 4. Clear the fields
            UICtrl.clearFields();
            // 5. Calculate and update budget
                //note: update in the two controllers
            updateBudget();
            updateExpPercentages();
        }
        
        
    };
    
    var deleteItem = function(event){
        var itemId = (event.target.parentElement.parentElement.parentElement.parentElement.id);
        
        if (itemId){
           //console.log(itemId);
            
            //1. delette from budget datastructure
            budgetController.deleteItem(itemId);
            //2. delete from ui
            UIController.deleteItem(itemId);
            //3. update the badget (ui + data)
            updateBudget();
            //4. update the percentages of the expenses
            updateExpPercentages();
            
        }
        
    };
    
    var updateExpPercentages = function(){    
        //to update in both ui and budget, but its done automatically in budet
        //so we need only here to take care of ui controller
        
        
        //1. get the percentages from budget controller
        var pers = budgetController.getExpensesPercentages();
        
        //2. ui update
        UIController.updateExpensesPercentages(pers);
        //console.log(pers)
    };
    
    
    return{
        init: function(){
            setEventListeners();
            //empty the topper of the page 
            UICtrl.updateBudget({
                sum:'',
                totalInc:'',
                totalExp:'',
                per:''
            });
            
            //
            UIController.displayMonth();
            console.log('app started.');
        }
    };
    
})(budgetController, UIController);


Controller.init();


